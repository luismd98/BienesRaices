import { Component, Input, OnInit } from '@angular/core';


import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-add-edit-owner',
  templateUrl: './add-edit-owner.component.html',
  styleUrls: ['./add-edit-owner.component.css']
})
export class AddEditOwnerComponent implements OnInit {

  constructor(private service:SharedService) { }

  @Input() owner:any;
  IdOwner:string = "";

  Name:string = "";
  Address:string ="";
  Birthday:string ="";

  Photo:string = "";
  PhotoFilePath:string = "";

  ngOnInit(): void {
      this.IdOwner = this.owner.IdOwner;
      this.Name = this.owner.Name;
      this.Address = this.owner.Address;
      this.Birthday = this.owner.Birthday;
      
      this.Photo = this.owner.Photo;
      this.PhotoFilePath = this.service.PhotoUrl+this.Photo;      
  }

  addOwner(){
    var val = { IdOwner: this.IdOwner,
                Name: this.Name,
                Address: this.Address,
                Birthday: this.Birthday,
                Photo: this.Photo };

    this.service.addOwner(val)
    .subscribe(res=>{ 
      alert(res.toString());
    },
    error => {
      alert(error["error"]);
    });
    
  }


  updateOwner(){
    var val = { IdOwner: this.IdOwner,
                Name: this.Name,
                Address: this.Address,
                Birthday: this.Birthday,
                Photo: this.Photo };
                
    this.service.updateOwner(val)
    .subscribe(success =>{
      alert(success.toString());
      },
      error => {
        alert(error["error"]);
      });
  }

  

  uploadPhoto(event:any){
    var file=event.target.files[0];
    const formData:FormData=new FormData();
    formData.append('uploadedFile',file,file.name);

    this.service.PhotoUpload(formData).subscribe((data:any)=>{
      this.Photo=data.toString();
      this.PhotoFilePath = this.service.PhotoUrl+this.Photo;
      
    },
    error => {
      alert(error["error"]);
    });
  }

}
