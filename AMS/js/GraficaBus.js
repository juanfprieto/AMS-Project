function GraficadorBus(d,mat,rt,ps,imd,ins,txtC,txtP,txtT,descuento,matNP,txtNP,tam,rec,txtInfo){
	this.Iniciado=false;
	this.Cantidad=0;
	this.divCant=txtC;
	this.divTotal=txtT;
	this.divPuesto=txtNP;
	this.divInfo=txtInfo;
	this.txtPrecio=txtP;
	this.descuento=descuento;
	this.posXP=0;
	this.posYP=0;
	this.tamano=tam;
	this.recorrido=rec;
	matB=mat.split(",");
	matP=matNP.split(",");
	if(ins.length>0)
		this.Inds=ins.split(",");
	else 
		this.Inds=null;
	if(matB.length<3)return;
	f=parseInt(matB[0]);
	c=parseInt(matB[1]);
	if(matB.length!=f*c+2)return;
	this.Cols=c;
	this.Fils=f;
	this.divCont=d;
	this.Ruta=rt;
	this.Post=ps;
	this.imgDir=imd;
	Pos=Array(f);
	Pst=Array(f);
	for(i=0;i<f;i++){
		Pst[i]=Array(c);
		Pos[i]=Array(c);
		for(j=0;j<c;j++){
			Pos[i][j]=parseInt(matB[j+i*c+2]);
			Pst[i][j]=parseInt(matP[j+i*c+2]);
		 }
	}
	this.Iniciado=true;
	
	this.Puesto=function(f,c){
		if(!this.Iniciado)return 0;
			return Pos[f][c];
	}
	
	this.Sel=function(f,c){
		this.SelN(f,c);
		this.Actualizar();
	}
	
	this.InfPstM=function(f,c,obj){
		/*if(Pst[f][c]>0){
			if(Pos[f][c]==2||Pos[f][c]==3)
			   this.divPuesto.innerHTML=Pst[f][c];
			else 
			   this.divPuesto.innerHTML=Pst[f][c];
			this.SPos(obj);
			this.divPuesto.style.left=this.posXP;
			this.divPuesto.style.top=this.posYP;
			this.divPuesto.style.display = "block";
		}
		else{
			this.divPuesto.innerHTML="";
			this.divPuesto.style.display = "none";
		}*/
		if(Pst[f][c]>0)this.divPuesto.innerHTML=Pst[f][c];
		else this.divPuesto.innerHTML="";
	}
	
	/*this.SPos=function(obj){
		this.posXP=this.posYP=0;
		while(obj != null){
			this.posYP+=obj.offsetTop;
			this.posXP+=obj.offsetLeft;
			obj=obj.offsetParent;
		}
	}*/
	
	this.InfPstO=function(){
		this.divPuesto.innerHTML="";
		//this.divPuesto.style.display = "none";
	}
	
	this.SelN=function(f,c){
		if(!this.Iniciado)
			return;
		if(Pos[f][c]==1){
			Pos[f][c]=5;
			this.Cantidad++;
		}
		else{
			Pos[f][c]=1;
			this.Cantidad--;
		}
	}	
	this.Lleno=function(){
		for(i=0;i<this.Fils;i++){
			for(j=0;j<this.Cols;j++){
				if(Pos[i][j]==5||Pos[i][j]==1)
					return(false);
			}
		}
		return(true);
	}
	this.Totales=function(){
		this.divCant.innerHTML=this.Cantidad;
		precioS=this.txtPrecio.value.replace(/\,/g,'');
		if(precioS.length==0 || isNaN(precioS))
			this.divTotal.innerHTML="0";
		else{
			if(this.descuento>0)tdesc=((parseFloat(precioS)*this.Cantidad)*(100-this.descuento))/100;
			else tdesc=(parseFloat(precioS)*this.Cantidad);
			this.divTotal.innerHTML=formatoValor(tdesc);
		}
	}
	
	this.Actualizar=function(){
		if(!this.Iniciado)
			return;
		t=0;
		dvHtml="";
		this.Cantidad=0;
		for(i=0;i<this.Fils;i++){
			for(j=0;j<this.Cols;j++){
				aA=aF=lI="";
				if(Pos[i][j]==1 || Pos[i][j]==5 || (Pos[i][j]+'').substring(0,1)=='3'){
					lI="onmouseover='grBus.InfPstM("+i+","+j+",this);' onmouseout='grBus.InfPstO();'";
					if(Pos[i][j]==1 || Pos[i][j]==5){
						aA="<a href='javascript:grBus.InfPstM("+i+","+j+",this);grBus.Sel("+i+","+j+");'>";
						if(Pos[i][j]==5)this.Cantidad++;
					}
					else
						aA="<a href='javascript:grBus.InfPstM("+i+","+j+",this);InfoVenta("+i+","+j+");'>";
					aF="</a>";
					
				}
				dvHtml+=aA + "<img "+lI+" src='"+this.imgDir+"sillaBus" + Pos[i][j] + ".jpg' name='Image"+t+"' border='0' height='"+this.tamano+"'>"+aF;
				t++;
			}
			dvHtml+="<br>";
		}
		this.divCont.innerHTML=dvHtml;
		this.Totales();
	}
	this.Confirmar=function(){
		if(!this.Iniciado)
			return;
		c=0;
		pars=this.Ruta;
		for(i=0;i<this.Fils;i++)
			for(j=0;j<this.Cols;j++)
				if(Pos[i][j]==5){
					c++;
					pars+="Z"+i+"Z"+j;}
		if(c==0)
			alert("No ha seleccionado ningun puesto!");
		else window.parent.location.href=this.Post+"&Pars="+pars;
	}

	this.Parametros=function(){
		if(!this.Iniciado)
		return("");
		c=0;
		pars="";
		for(i=0;i<this.Fils;i++)
		for(j=0;j<this.Cols;j++)
			if(Pos[i][j]==5){
				c++;
				pars+=i+"-"+j+"|";
			}
		if(c==0)pars="";
		return(pars);
	}

	this.Crear=function(){
		if(!this.Iniciado)return;
			c=0;
			pars="";
			for(i=0;i<this.Fils;i++)
			for(j=0;j<this.Cols;j++)
				pars+=Pos[i][j];
			window.parent.location.href=this.Post+"&Pars="+pars;
	}
	
	this.Tomar=function(puestos){
		var pLibres=0;
		for(i=0;i<this.Fils;i++)
			for(j=0;j<this.Cols;j++)
				if(Pos[i][j]==1||Pos[i][j]==5){
					pLibres++;
					Pos[i][j]=1;
				}
		this.Actualizar();
		if(pLibres<puestos){
			alert('No hay suficientes puestos libres.')
			return;
		}
		pLibres=0;
		for(iT=0;iT<this.Fils;iT++)
			for(jT=0;jT<this.Cols;jT++)
				if(Pos[iT][jT]==1){
					this.SelN(iT,jT);
					this.Actualizar();
					pLibres++;
					if(pLibres>=puestos)return;
				}
	}
}
