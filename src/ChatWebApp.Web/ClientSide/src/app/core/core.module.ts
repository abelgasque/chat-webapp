import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { CoreRoutingModule } from './core-routing.module';
import { CoreComponent } from './core.component';
import { LayoutAdminComponent } from '../layouts/layout-admin/layout-admin.component';
import { LayoutUserComponent } from '../layouts/layout-user/layout-user.component';
import { HomeModule } from '../modules/home/home.module';

@NgModule({
  declarations: [
    CoreComponent,
    LayoutAdminComponent,
    LayoutUserComponent
  ],
  imports: [
    CommonModule,
    RouterModule,

    CoreRoutingModule,
    HomeModule
  ]
})
export class CoreModule { }
