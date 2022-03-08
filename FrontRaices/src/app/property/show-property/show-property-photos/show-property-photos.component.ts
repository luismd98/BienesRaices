import { Component, Input, OnInit } from '@angular/core';

import { SharedService } from 'src/app/shared.service';


@Component({
  selector: 'app-show-property-photos',
  templateUrl: './show-property-photos.component.html',
  styleUrls: ['./show-property-photos.component.css']
})
export class ShowPropertyPhotosComponent implements OnInit {

  constructor(private service:SharedService) { }

  @Input() property:any;

  PropertyImage:any;
  PropertyImageList:any = [];

  Photo:string = "";
  PhotoFilePath:string = "";

  ngOnInit(): void {

    this.PropertyImage = {
      IdProperty:0,
      IdPropertyImage: ""
    };
    this.refreshPropertyImagesList();
  }

  uploadPhoto(event:any){
    var file = event.target.files[0];

    const formData:FormData=new FormData();
    formData.append('uploadedFile',file,file.name);
    formData.append('IdProperty',this.property.IdProperty);

    this.service.PhotoPropertyUpload(formData).subscribe((data:any)=>{
      this.Photo=data.toString();
      this.PhotoFilePath = this.service.PhotoUrl+this.Photo;
      this.refreshPropertyImagesList();
      alert(data.toString());
    },
    error => {
      alert(error["error"]);
    });
  }
  

  hidePicture(event:any){
    if(confirm('Do you want to DELETE this picture?')){      
      this.PropertyImage.IdPropertyImage = event.target.attributes.value["value"];
      this.service.hidePicture(this.PropertyImage).subscribe(data=>{
        alert(data.toString());
        this.refreshPropertyImagesList();
      },
      error => {
        alert(error["error"]);
      });
    }
  }

  refreshPropertyImagesList() {
    var IdProperty:number =  this.property.IdProperty;

    this.service.getPropertyImages(IdProperty).subscribe(data => {
      this.PropertyImageList = data;

      let path = this.service.getPhotoPath() + "property/";
      for(let i=0; i<this.PropertyImageList.length; i++){
        this.PropertyImageList[i].PhotoPath = path + this.PropertyImageList[i].Photo;
        console.log(this.PropertyImageList[i]);
      };
    },
    error => {
      alert(error["error"]);
    });

    this.PropertyImageList.forEach((element: any) => console.log(element));
  }

  closeClick(){
    this.refreshPropertyImagesList();
  }

}
