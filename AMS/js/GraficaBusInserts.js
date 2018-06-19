function GraficadorBus(d,mat,rt,ps,imd,tp,ins,obj)
{
    this.Iniciado=false;
    matB=mat.split(",");
    if(ins.length>0)
        this.Inds=ins.split(",");
    else this.Inds=null;
    if(matB.length<3)
        return;
    f=parseInt(matB[0]);
    c=parseInt(matB[1]);
    this.Tipo=tp;
    of=tp;
    if(of>1||of<0)
        return;
    if(of==1)
        mc=parseInt(matB[2]);
    if(matB.length!=f*c+2+of)
        return;
    this.Cols=c;
    this.Fils=f;
    this.divCont=d;
    this.Ruta=rt;
    this.Post=ps;
    this.imgDir=imd;
    this.ParsTwo = "";
    this.Num = 0;
    this.Obj=obj;
    Pos=Array(f);
    for(i=0;i<f;i++)
    {
        Pos[i]=Array(c);
        for(j=0;j<c;j++)
            Pos[i][j]=parseInt(matB[j+i*c+2+of]);
    }
    this.Iniciado=true;
    this.Puesto=function(f,c)
    {
        if(!this.Iniciado)
            return 0;
    return Pos[f][c];
    };
    //////////////////////////////////////////////////////////////
    this.Sel= function(f,c)
    {
        if(!this.Iniciado)
            return;
        if(this.Tipo==0)
        {
            if(Pos[f][c]==1)
                Pos[f][c]=5;
            else
                Pos[f][c]=1;
        }
        if(this.Tipo==1)
        {
            t=this.Inds.length;
            p=0;
            for(n=0;n<t;n++)
                if(parseInt(this.Inds[n])==Pos[f][c])
                    p=n;
            p=p+1;
            if(p>t-1)
                p=0;
            if(p==1)
            {
                if(this.Obj.value!="")
                {
                    ////////////////////////////////
                    if(this.Num==0)
                    {
                        this.ParsTwo += f+"-"+c+"-"+this.Obj.value;
                        this.Num = this.Num+1;
                    }
                    else
                    {
                        this.ParsTwo += "-"+f+"-"+c+"-"+this.Obj.value;
                    }
                    this.Obj.value = "";
                    //////////////////////////////////
                }
                else
                {
                    p=2;
                    alert("Debe digitar el número del puesto!")
                }
            }
            Pos[f][c]=parseInt(this.Inds[p]);
        }
        this.Actualizar();
    }
    ///////////////////////////////////////////////////////////////////////////
    this.Actualizar=function(){if(!this.Iniciado)return;t=0;dvHtml="";for(i=0;i<this.Fils;i++){for(j=0;j<this.Cols;j++){opn=aA=aF="";if((Pos[i][j]==1||Pos[i][j]==5)||this.Tipo==1){aA="<a href='javascript:grBus.Sel("+i+","+j+");'>";aF="</a>";}dvHtml+=aA + "<img src='"+this.imgDir+"sillaBus" + Pos[i][j] + ".jpg' name='Image"+t+"' border='"+this.Tipo+"'>"+aF;t++;}dvHtml+="<br>";}this.divCont.innerHTML=dvHtml;}
    //////////////////////////////////////////////////////////////////////////
    this.Confirmar=function()
    {
        if(!this.Iniciado)
            return;
        c=0;
        pars=this.Ruta;
        for(i=0;i<this.Fils;i++)
            for(j=0;j<this.Cols;j++)
                if(Pos[i][j]==5)
                    {
                        c++;pars+="Z"+i+"Z"+j;}
                        if(c==0)
                            alert("No ha seleccionado nungun puesto!");
                        else window.parent.location.href=this.Post+"&Pars="+pars;
                    }

    this.Crear=function(){if(!this.Iniciado)return;c=0;pars="";for(i=0;i<this.Fils;i++)for(j=0;j<this.Cols;j++)pars+=Pos[i][j];chr="&";window.parent.location.href=this.Post+chr+"Pars="+pars+"&ParsTwo="+this.ParsTwo+"";}
}
