import { Injectable } from '@angular/core';

//imported libraries 
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  readonly APIUrl = "https://localhost:44314";
  readonly PhotoUrl = "https://localhost:44314/Photos/";

  //instanciate / initiate  HttpClient object 
  constructor(private http:HttpClient) { }

  //Calling API methods | Owner

  getOwnerList():Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/Owner');
  }

  addOwner(val:any){
    return this.http.post(this.APIUrl+'/Owner',val)
  }

  updateOwner(val:any){
    return this.http.put(this.APIUrl+'/Owner',val)
  }

  //Properties

  getPropertyList():Observable<any[]>{
    return this.http.get<any>(this.APIUrl+'/Property');
  }

  addProperty(val:any){
    return this.http.post(this.APIUrl+'/Property',val)
  }

  updateProperty(val:any){
    return this.http.put(this.APIUrl+'/Property',val)
  }


  //Property trace
  confirmSale(val:any){
    return this.http.post(this.APIUrl+'/PropertyTrace',val)
  }

  getPropertyTrace(val:number){
    return this.http.get(this.APIUrl+'/PropertyTrace/'+val)
  }

  // property image
  getPropertyImages(val:number){
    return this.http.get(this.APIUrl+'/PropertyImage/'+val)
  }
  
  hidePicture(val:any){
    return this.http.put(this.APIUrl+'/PropertyImage',val)
  }

  //Upload photo

  PhotoUpload(val:any){
    return this.http.post(this.APIUrl+'/PhotoUpload',val)
  }

  PhotoPropertyUpload(val:any){
    return this.http.post(this.APIUrl+'/PhotoUpload/SavePropertyFile',val)
  }

  getPhotoPath(){
    return this.PhotoUrl;
  }

  
}
