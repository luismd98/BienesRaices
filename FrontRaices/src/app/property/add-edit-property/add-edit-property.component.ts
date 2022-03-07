import { Component, Input, OnInit } from '@angular/core';

import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-add-edit-property',
  templateUrl: './add-edit-property.component.html',
  styleUrls: ['./add-edit-property.component.css']
})
export class AddEditPropertyComponent implements OnInit {

  constructor(private service:SharedService) { }

  @Input() property:any;

  IdProperty:string = "";

  Name:string = "";
  Address:string ="";
  Price:number = 0;
  CodeInternal:number = 0;
  Year:string = "";
  IdOwner: string = "";


  Photo:string = "";
  PhotoFilePath:string = "";

  OwnerList:any = [];

  ngOnInit(): void {

    this.service.getOwnerList().subscribe((data:any)=>{
      this.OwnerList = data;

      this.IdProperty = this.property.IdProperty;
      this.Name = this.property.Name;
      this.Address = this.property.Address;
      this.Price = this.property.Price;
      this.CodeInternal = this.property.CodeInternal;
      this.Year = this.property.Year;
      this.IdOwner = this.property.IdOwner;
      
      this.Photo = this.property.Photo;
      this.PhotoFilePath = this.service.PhotoUrl+this.Photo;
    });            
  }

  addProperty(){
    var val = { IdProperty: this.IdProperty,
                Name: this.Name,
                Address: this.Address,
                Price: this.Price,
                CodeInternal: this.CodeInternal,
                Year: this.Year,
                IdOwner: this.IdOwner,
                Photo: this.Photo };

    this.service.addProperty(val).subscribe(res=>{
      alert(res.toString());
    });
    
  }


  updateProperty(){
    var val = { IdProperty: this.IdProperty,
                Name: this.Name,
                Address: this.Address,
                Price: this.Price,
                CodeInternal: this.CodeInternal,
                Year: this.Year,
                IdOwner: this.IdOwner,
                Photo: this.Photo };
                
    this.service.updateProperty(val).subscribe(res=>{
      alert(res.toString());
    });
  }


  uploadPhoto(event:any){
    var file=event.target.files[0];
    const formData:FormData=new FormData();
    formData.append('uploadedFile',file,this.Photo);

    this.service.PhotoUpload(formData).subscribe((data:any)=>{
      this.Photo=data.toString();
      this.PhotoFilePath = this.service.PhotoUrl+this.Photo;
      
    });
  }

}
