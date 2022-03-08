import { Component, OnInit } from '@angular/core';

import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-show-property',
  templateUrl: './show-property.component.html',
  styleUrls: ['./show-property.component.css']
})
export class ShowPropertyComponent implements OnInit {

  constructor(private service:SharedService) { }

  //Variables
  PropertiesList:any=[];

  ModalTitle: string = "";
  ActivateAddEditPropertiesComponent:boolean = false;
  ActivateSellPropertyComponent:boolean = false;
  ActivatePropertyTraceComponent:boolean = false;
  ActivatePropertyPhotosComponent:boolean = false;
  
  property:any;  

  //Filters / sort
  IdPropertyFilter:string ="";
  OwnerNameFilter:string = "";
  PropertiesListWithoutFilter: any=[];


  ngOnInit(): void {
    this.refreshPropertiesList();
  }  
  
  // view pictures button

  picturesView(item:any){
    this.property = item;
    this.ModalTitle =   "Property images - " + 
                        this.property.Address + 
                        " (" +
                        this.property.CodeInternal+
                        ")";
    this.ActivatePropertyPhotosComponent = true;
  }

  //Add / edit components
  
  addClick(){
    this.property = {
      IdProperty: 0,
      Name: "",
      Address: "",      
      Price: 0,
      CodeInternal: 0,
      Year: "",
      IdOwner: 0,
      OwnerName: ""
    };
    this.ModalTitle = "Register new property";
    this.ActivateAddEditPropertiesComponent = true;
  }

  editClick(item: any){
    this.property = item;
    this.ModalTitle = "Edit property details";
    this.ActivateAddEditPropertiesComponent = true;
  }


  //Sell component
  sellClick(item: any){
    this.property = item;
    this.ModalTitle = "Sell property";
    this.ActivateSellPropertyComponent = true;
  }

  traceClick(item: any){
    this.property = item;
    this.ModalTitle = "Property Trace";
    this.ActivatePropertyTraceComponent = true;
  }



  //Close

  closeClick(){
    this.ActivateSellPropertyComponent = false;
    this.ActivateAddEditPropertiesComponent = false;
    this.ActivatePropertyTraceComponent = false;
    this.ActivatePropertyPhotosComponent = false;
    this.refreshPropertiesList();
  }

  //Sort and filter

  FilterList(){
    var IdPropertyFilter = this.IdPropertyFilter;
    var OwnerNameFilter = this.OwnerNameFilter;

    if(this.IdPropertyFilter){
      this.PropertiesList = this.PropertiesListWithoutFilter.filter(function(data:any){      
        return data.IdProperty.toString().toLowerCase().includes(
          IdPropertyFilter.toString().trim().toLowerCase()
        )&&
        data.OwnerName.toString().toLowerCase().includes(
          OwnerNameFilter.toString().trim().toLowerCase()
        )
      }); 
    }else{
      this.PropertiesList = this.PropertiesListWithoutFilter.filter(function(data:any){      
        return data.OwnerName.toString().toLowerCase().includes(
          OwnerNameFilter.toString().trim().toLowerCase()
        )
      }); 
    }

    
  }

  sortResult(data: any, isAscending: boolean){
    this.PropertiesList = this.PropertiesListWithoutFilter.sort(function (a:any, b:any){
      if(isAscending){
        return (a[data] > b[data]) ? 1 : (a[data] < b[data] ? -1 :0 );
      }else{
        return (b[data] > a[data]) ? 1 : (b[data] < a[data] ? -1 :0 );
      }
    })
  }

  //Refresh

  refreshPropertiesList(){
    this.service.getPropertyList().subscribe(data => {
      this.PropertiesList = data;
      this.PropertiesListWithoutFilter = data;
    },
    error => {
      alert(error["error"]);
    });
  }


}

