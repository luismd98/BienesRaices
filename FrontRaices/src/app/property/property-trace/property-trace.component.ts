import { Component, Input, OnInit } from '@angular/core';

import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-property-trace',
  templateUrl: './property-trace.component.html',
  styleUrls: ['./property-trace.component.css']
})
export class PropertyTraceComponent implements OnInit {

  constructor(private service:SharedService) { }

  @Input() property:any;

  //Variables
  PropertiesTraceList:any=[];

  //Filters / sort
  IdPropertyFilter:string ="";
  PropertyNameFilter:string = "";
  PropertiesListWithoutFilter: any=[];

  ngOnInit(): void {
    this.refreshPropertiesList();
  }  
  
  //Close btn

  closeClick(){
    this.refreshPropertiesList();
  }

  //Sort and filter

  FilterList(){
    var IdPropertyFilter = this.IdPropertyFilter;
    var PropertyNameFilter = this.PropertyNameFilter;

    this.PropertiesTraceList = this.PropertiesListWithoutFilter.filter(function(data:any){
      return data.IdOwner.toString().toLowerCase().includes(
        IdPropertyFilter.toString().trim().toLowerCase()
      )&&
      data.Name.toString().toLowerCase().includes(
        PropertyNameFilter.toString().trim().toLowerCase()
      )
    }); 
  }

  sortResult(data: any, isAscending: boolean){
    this.PropertiesTraceList = this.PropertiesListWithoutFilter.sort(function (a:any, b:any){
      if(isAscending){
        return (a[data] > b[data]) ? 1 : (a[data] < b[data] ? -1 :0 );
      }else{
        return (b[data] > a[data]) ? 1 : (b[data] < a[data] ? -1 :0 );
      }
    })
  }


  verifyData(){
    var result=this.PropertiesTraceList.filter(function(pList: any){
      return pList.IdPropertyTrace;    
    });
    return result.length!=0;
  }

  //Refresh

  refreshPropertiesList(){
    var IdProperty:number =  this.property.IdProperty;

    this.service.getPropertyTrace(IdProperty).subscribe(data => {
      this.PropertiesTraceList = data;
      this.PropertiesListWithoutFilter = data;
    },
    error => {
      alert(error["error"]);
    });


    this.PropertiesTraceList.forEach((element: any) => console.log(element));


  }


}
