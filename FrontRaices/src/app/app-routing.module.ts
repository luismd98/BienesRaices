import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

//import created components
import { OwnerComponent } from './owner/owner.component';
import { PropertyComponent } from './property/property.component';

const routes: Routes = [
  {path: 'owner', component:OwnerComponent},
  {path: 'property', component:PropertyComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
