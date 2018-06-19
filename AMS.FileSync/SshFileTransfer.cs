using System;
using System.Collections;
using Tamir.SharpSsh;
using Tamir.SharpSsh.jsch.examples;
using System.IO;

namespace AMS.FileSync
{
    public class OneWayFileSyncSSH
    {
        private string localFile;
        private string remoteFolder;

        private SshTransferProtocolBase sshCp;

        public OneWayFileSyncSSH(string host, string user, string password, string localFile, string remoteFolder)
        {
            this.localFile = localFile;
            this.remoteFolder = remoteFolder;

            sshCp = new Tamir.SharpSsh.Sftp(host, user);
            sshCp.Password = password;
            sshCp.Connect();
        }

        public OneWayFileSyncSSH(SshTransferProtocolBase sshCp, string localFile, string remoteFolder)
        {
            this.localFile = localFile;
            this.remoteFolder = remoteFolder;
            this.sshCp = sshCp;
        }

        public void syncFile()
        {
            sshCp.Put(localFile, remoteFolder);
        }

        public void closeConnection()
        {
            sshCp.Close();
        }
    }

    public class OneWayFolderSyncSSH
    {
        private string localFolder;
        private string remoteFolder;

        private ArrayList localFiles;
        private ArrayList remoteFiles;

        private SshExec exec;
        private SshTransferProtocolBase sshCp;

        public OneWayFolderSyncSSH(string host, string user, string password, string localFolder, string remoteFolder)
        {
            this.localFolder = localFolder;
            this.remoteFolder = remoteFolder;

            this.localFiles = new ArrayList();
            this.remoteFiles = new ArrayList();

            exec = new SshExec(host, user);
            sshCp = new Tamir.SharpSsh.Sftp(host, user);

            exec.Password = password;
            sshCp.Password = password;

            exec.Connect();
            sshCp.Connect();
        }

        public OneWayFolderSyncSSH(SshExec exec, SshTransferProtocolBase sshCp, string localFolder, string remoteFolder)
        {
            this.localFolder = localFolder;
            this.remoteFolder = remoteFolder;

            this.localFiles = new ArrayList();
            this.remoteFiles = new ArrayList();

            this.exec = exec;
            this.sshCp = sshCp;
        }

        public void syncFiles(string fileExt)
        {
            fillLocalFiles(fileExt);
            fillRemoteFiles();

            foreach (SimpleComparisonFile file in remoteFiles)
                if(!localFiles.Contains(file))
                    exec.RunCommand(String.Format("rm {0}/{1}", remoteFolder, file.fileName));

            foreach (SimpleComparisonFile file in localFiles)
                if (!remoteFiles.Contains(file))
                    sshCp.Put(String.Format("{0}{1}", localFolder, file.fileName), String.Format("{0}", remoteFolder));
        }

        private void fillLocalFiles(string fileExt)
        {
            string[] filePaths = Directory.GetFiles(localFolder, fileExt);

            for (int i=0; i < filePaths.Length; i++)
            {
                FileInfo fileInfo = new FileInfo(filePaths[i]);                
                localFiles.Add(new SimpleComparisonFile(fileInfo.Name, fileInfo.Length));
            }
        }

        private void fillRemoteFiles()
        {
            string output = exec.RunCommand(String.Format("ls -l {0} | awk '{1}'", remoteFolder, "{print $8, $5}"));

            foreach (string file in output.Split('\n'))
            {
                if (file == "" || file == " ") continue;
                string[] fileStats = file.Split(' ');
                remoteFiles.Add(new SimpleComparisonFile(fileStats[0], Convert.ToInt64(fileStats[1])));
            }

        }

        public void closeConnection()
        {
            exec.Close();
            sshCp.Close();
        }

        class SimpleComparisonFile
        {
            public string fileName;
            public long fileSize;

            public SimpleComparisonFile(string fileName, long fileSize)
            {
                this.fileName = fileName;
                this.fileSize = fileSize;
            }

            public override bool Equals(object obj)
            {
                SimpleComparisonFile file = (SimpleComparisonFile)obj;
                return this.fileName == file.fileName && this.fileSize == file.fileSize;
            }

            public override string ToString()
            {
                return String.Format("[{0} - {1}]", fileName, fileSize);
            }
        }

    }
}
