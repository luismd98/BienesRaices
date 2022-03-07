import { Component, Input, OnInit } from '@angular/core';
import {formatDate} from '@angular/common';

import { SharedService } from 'src/app/shared.service';

@Component({
  selector: 'app-sell-property',
  templateUrl: './sell-property.component.html',
  styleUrls: ['./sell-property.component.css']
})
export class SellPropertyComponent implements OnInit {

  constructor(private service:SharedService) { }

  @Input() property:any;

  //Property values
  IdProperty:string = "";

  Name:string = "";
  Address:string ="";
  Price:number = 0;
  CodeInternal:number = 0;
  Year:string = "";
  IdOwner: string = "";

  //property *tracing* values

  DateSale:string = "";
  Tax:number = 0;

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
      this.DateSale = formatDate(new Date(), 'yyyy-MM-dd', 'en');
      this.Tax = 19; //default value
    });            
  }

  confirmSale(){
    var val = { 
      DateSale: this.DateSale,
      Name: this.Name,
      Value: this.Price,      
      Tax: this.Tax,
      IdProperty: this.IdProperty,
      IdOwner: this.IdOwner
    };

    this.service.confirmSale(val).subscribe(res=>{
      alert(res.toString());
    });
  }


}
