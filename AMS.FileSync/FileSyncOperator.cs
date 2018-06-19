using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Tamir.SharpSsh;
using EO.Web;
using Microsoft.Synchronization.Files;
using Microsoft.Synchronization;

namespace AMS.FileSync
{
    public class FileSyncOperator
    {
        private static string kioskoRepository = ConfigurationManager.AppSettings["KioskoRepository"];
        private static string kioskoPathToXML = ConfigurationManager.AppSettings["KioskoPathToConfig"];
        private static string kioskoPathToImages = ConfigurationManager.AppSettings["KioskoPathToImages"];
        private static string kioskoPathToVideos = ConfigurationManager.AppSettings["KioskoPathToVideos"];
        private static string kioskoUser = ConfigurationManager.AppSettings["KioskoUser"];
        private static string kioskoPass = ConfigurationManager.AppSettings["KioskoPass"];
        private static string pathToTemp = ConfigurationManager.AppSettings["PathToTemp"];

        public static void syncKioskoLinux(string host, string imageFolder, string videoFolder, string numTienda, 
                                        ProgressTaskEventArgs e, int progActual, int razonCambio)
        {
            SshExec exec;
            SshTransferProtocolBase sshCp;

            exec = new SshExec(host, kioskoUser);
            sshCp = new Sftp(host, kioskoUser);
            exec.Password = kioskoPass;
            sshCp.Password = kioskoPass;

            progActual += razonCambio;
            exec.Connect();
            e.UpdateProgress(progActual);

            progActual += razonCambio;
            sshCp.Connect();
            e.UpdateProgress(progActual);

            string imageLocalPath = String.Format("{0}{1}", kioskoRepository, imageFolder);
            string videoLocalPath = String.Format("{0}{1}", kioskoRepository, videoFolder);
            string xmlLocalPath = String.Format("{0}{1}", pathToTemp, "kioskoXML");

            OneWayFolderSyncSSH imageSync = new OneWayFolderSyncSSH(exec, sshCp, imageLocalPath, kioskoPathToImages);
            OneWayFolderSyncSSH videoSync = new OneWayFolderSyncSSH(exec, sshCp, videoLocalPath, kioskoPathToVideos);
            OneWayFolderSyncSSH xmlSync = new OneWayFolderSyncSSH(exec, sshCp, xmlLocalPath, kioskoPathToXML);
            XmlKioskoLinux xmlKiosko = new XmlKioskoLinux(kioskoPathToImages, kioskoPathToVideos, pathToTemp, numTienda, exec);

            progActual += razonCambio * 2;
            imageSync.syncFiles("*.jpg");
            e.UpdateProgress(progActual);

            progActual += razonCambio * 2;
            videoSync.syncFiles("*.flv");
            e.UpdateProgress(progActual);

            progActual += razonCambio;
            xmlKiosko.generateServerXML(host);
            xmlKiosko.generateImageXML();
            xmlKiosko.generateVideoXML();
            e.UpdateProgress(progActual);

            progActual += razonCambio;
            xmlSync.syncFiles("*.xml");
            e.UpdateProgress(progActual);

            progActual += razonCambio;
            exec.Close();
            sshCp.Close();
            e.UpdateProgress(progActual);
        }

        public static void syncKioskoWindows(string host, string imageFolder, string videoFolder, string numTienda,
                                        ProgressTaskEventArgs e, int progActual, int razonCambio)
        {
            string imageLocalPath = String.Format("{0}{1}", kioskoRepository, imageFolder);
            string videoLocalPath = String.Format("{0}{1}", kioskoRepository, videoFolder);
            string xmlLocalPath = String.Format("{0}{1}", pathToTemp, "kioskoXML\\");

            string imageKioskoPath = String.Format("\\\\{0}\\{1}", host, kioskoPathToImages);
            string videoKioskoPath = String.Format("\\\\{0}\\{1}", host, kioskoPathToVideos);
            string xmlKioskoPath = String.Format("\\\\{0}\\{1}", host, kioskoPathToXML);

            XmlKioskoWindows xmlKiosko = new XmlKioskoWindows(imageKioskoPath, videoKioskoPath, xmlLocalPath, numTienda);

            try
            {
                FileSyncOptions options = 
                    FileSyncOptions.ExplicitDetectChanges |
                    FileSyncOptions.RecycleDeletedFiles | 
                    FileSyncOptions.RecyclePreviousFileOnUpdates |
                    FileSyncOptions.RecycleConflictLoserFiles;

                FileSyncScopeFilter filterImages = new FileSyncScopeFilter();
                filterImages.FileNameIncludes.Add("*.jpg");
                FileSyncScopeFilter filterVideos = new FileSyncScopeFilter();
                filterImages.FileNameIncludes.Add("*.flv");
                FileSyncScopeFilter filterXML = new FileSyncScopeFilter();
                filterImages.FileNameIncludes.Add("*.xml");

                DetectChangesOnFileSystemReplica(imageLocalPath, filterImages, options);
                DetectChangesOnFileSystemReplica(imageKioskoPath, filterImages, options);

                DetectChangesOnFileSystemReplica(videoLocalPath, filterImages, options);
                DetectChangesOnFileSystemReplica(videoKioskoPath, filterImages, options);

                progActual += razonCambio * 2;
                e.UpdateProgress(progActual);

                SyncFileSystemReplicasOneWay(imageLocalPath, imageKioskoPath, null, options);
                SyncFileSystemReplicasOneWay(imageKioskoPath, imageLocalPath, null, options);
                progActual += razonCambio * 2;
                e.UpdateProgress(progActual);

                SyncFileSystemReplicasOneWay(videoLocalPath, videoKioskoPath, null, options);
                SyncFileSystemReplicasOneWay(videoKioskoPath, videoLocalPath, null, options);
                progActual += razonCambio * 2;
                e.UpdateProgress(progActual);

                xmlKiosko.generateServerXML(host);
                xmlKiosko.generateImageXML();
                xmlKiosko.generateVideoXML();
                progActual += razonCambio;
                e.UpdateProgress(progActual);

                DetectChangesOnFileSystemReplica(xmlLocalPath, filterImages, options);
                DetectChangesOnFileSystemReplica(xmlKioskoPath, filterImages, options);

                SyncFileSystemReplicasOneWay(xmlLocalPath, xmlKioskoPath, null, options);
                SyncFileSystemReplicasOneWay(xmlKioskoPath, xmlLocalPath, null, options);
                progActual += razonCambio * 2;
                e.UpdateProgress(progActual);
            }
            catch (Exception ex)
            {
                e.UpdateProgress(progActual, String.Format(".{0}\n{1}", ex.Message, ex.InnerException));
            }
        }

        // Create a provider, and detect changes on the replica that the provider
        // represents.
        public static void DetectChangesOnFileSystemReplica(
                string replicaRootPath,
                FileSyncScopeFilter filter, FileSyncOptions options)
        {
            FileSyncProvider provider = null;
            //FileSyncProvider provider;

            try
            {
                //3provider = new FileSyncProvider("C:\\inetpub\\wwwroot\\MAKRO\\Repository");
                provider = new FileSyncProvider(replicaRootPath, filter, options);
                //1provider = new FileSyncProvider(replicaRootPath);
                provider.DetectChanges();
            }
            finally
            {
                // Release resources.
                if (provider != null)
                    provider.Dispose();
            }
        }

        public static void SyncFileSystemReplicasOneWay(
                string sourceReplicaRootPath, string destinationReplicaRootPath,
                FileSyncScopeFilter filter, FileSyncOptions options)
        {
            FileSyncProvider sourceProvider = null;
            FileSyncProvider destinationProvider = null;

            try
            {
                // Instantiate source and destination providers, with a null filter (the filter
                // was specified in DetectChangesOnFileSystemReplica()), and options for both.
                sourceProvider = new FileSyncProvider(
                    sourceReplicaRootPath, filter, options);
                destinationProvider = new FileSyncProvider(
                    destinationReplicaRootPath, filter, options);

                // Register event handlers so that we can write information
                // to the console.
                //destinationProvider.AppliedChange +=
                //    new EventHandler<AppliedChangeEventArgs>(OnAppliedChange);
                //destinationProvider.SkippedChange +=
                //    new EventHandler<SkippedChangeEventArgs>(OnSkippedChange);

                // Use SyncCallbacks for conflicting items.
                //SyncCallbacks destinationCallbacks = destinationProvider.DestinationCallbacks;
                //destinationCallbacks.ItemConflicting += new EventHandler<ItemConflictingEventArgs>(OnItemConflicting);
                //destinationCallbacks.ItemConstraint += new EventHandler<ItemConstraintEventArgs>(OnItemConstraint);

                SyncOrchestrator agent = new SyncOrchestrator();
                agent.LocalProvider = sourceProvider;
                agent.RemoteProvider = destinationProvider;
                agent.Direction = SyncDirectionOrder.Upload; // Upload changes from the source to the destination.

                Console.WriteLine("Synchronizing changes to replica: " +
                    destinationProvider.RootDirectoryPath);
                agent.Synchronize();
            }
            finally
            {
                // Release resources.
                if (sourceProvider != null) sourceProvider.Dispose();
                if (destinationProvider != null) destinationProvider.Dispose();
            }
        }
    }
}