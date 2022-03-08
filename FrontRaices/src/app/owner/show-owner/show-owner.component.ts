import { Component, OnInit } from '@angular/core';


import { SharedService } from 'src/app/shared.service';
import { __assign } from 'tslib';

@Component({
  selector: 'app-show-owner',
  templateUrl: './show-owner.component.html',
  styleUrls: ['./show-owner.component.css']
})
export class ShowOwnerComponent implements OnInit {

  //new object and initialization of the shared service
  constructor(private service:SharedService) { }

  //Variables
  OwnerList:any=[];

  ModalTitle: string = "";
  ActivateAddEditOwnerComponent:boolean = false;
  owner:any;

  //Filters / sort
  OwnerIdFilter:string ="";
  OwnerNameFilter:string = "";
  OwnerListWithoutFilter: any=[];


  ngOnInit(): void {
    this.refreshOwnerList();
  }
  

  addClick(){
    this.owner = {
      IdOwner: 0,
      Name: "",
      Address: "",      
      Birthday: "",
      Photo: "anon.png"
    };
    this.ModalTitle = "Register new owner";
    this.ActivateAddEditOwnerComponent = true;
  }

  editClick(item: any){
    this.owner = item;
    this.ModalTitle = "Edit owner details";
    this.ActivateAddEditOwnerComponent = true;
  }

  closeClick(){
    this.ActivateAddEditOwnerComponent = false;
    this.refreshOwnerList();
  }

  //Sort and filter

  FilterList(){
    var OwnerIdFilter = this.OwnerIdFilter;
    var OwnerNameFilter = this.OwnerNameFilter;

    this.OwnerList = this.OwnerListWithoutFilter.filter(function(data:any){
      return data.IdOwner.toString().toLowerCase().includes(
        OwnerIdFilter.toString().trim().toLowerCase()
      )&&
      data.Name.toString().toLowerCase().includes(
        OwnerNameFilter.toString().trim().toLowerCase()
      )
    }); 
  }

  sortResult(data: any, isAscending: boolean){
    this.OwnerList = this.OwnerListWithoutFilter.sort(function (a:any, b:any){
      if(isAscending){
        return (a[data] > b[data]) ? 1 : (a[data] < b[data] ? -1 :0 );
      }else{
        return (b[data] > a[data]) ? 1 : (b[data] < a[data] ? -1 :0 );
      }
    })
  }
  

  //Refresh

  refreshOwnerList(){
    this.service.getOwnerList().subscribe((data: any=[]) => {
      this.OwnerList = data;
      this.OwnerListWithoutFilter = data;
      
      //Add the value: PhotoPath to include file url
      let path = this.service.getPhotoPath();
      for(let i=0; i<this.OwnerList.length; i++){
        this.OwnerList[i].PhotoPath = path + this.OwnerList[i].Photo;
      };

    },
    error => {
      alert("Data could not be loaded, check the database connection")
    });
    
  }


}
