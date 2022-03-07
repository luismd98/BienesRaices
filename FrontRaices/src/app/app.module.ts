import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { OwnerComponent } from './owner/owner.component';
import { PropertyComponent } from './property/property.component';


//imported provider
import { SharedService } from './shared.service';

//imported modules
import {HttpClientModule} from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ShowOwnerComponent } from './owner/show-owner/show-owner.component';
import { AddEditOwnerComponent } from './owner/add-edit-owner/add-edit-owner.component';
import { ShowPropertyComponent } from './property/show-property/show-property.component';
import { AddEditPropertyComponent } from './property/add-edit-property/add-edit-property.component';
import { SellPropertyComponent } from './property/sell-property/sell-property.component';
import { PropertyTraceComponent } from './property/property-trace/property-trace.component';
import { FallbackImgDirective } from './owner/show-owner/fallback-img.directive';
import { ShowPropertyPhotosComponent } from './property/show-property/show-property-photos/show-property-photos.component';

@NgModule({
  declarations: [
    AppComponent,
    OwnerComponent,
    PropertyComponent,
    ShowOwnerComponent,
    AddEditOwnerComponent,
    ShowPropertyComponent,
    AddEditPropertyComponent,
    SellPropertyComponent,
    PropertyTraceComponent,
    FallbackImgDirective,
    ShowPropertyPhotosComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule, 
    
    HttpClientModule,
    FormsModule, 
    ReactiveFormsModule
  ],
  providers: [SharedService],
  bootstrap: [AppComponent]
})
export class AppModule { }
