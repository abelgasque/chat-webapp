import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CoreComponent } from './core.component';

import { LayoutAdminComponent } from '../layouts/layout-admin/layout-admin.component';
import { LayoutUserComponent } from '../layouts/layout-user/layout-user.component';

const routes: Routes = [
  {
    path: '',
    component: CoreComponent,
    children: [
      {
        path: 'home',
        loadChildren: () => import('src/app/modules/home/home.module').then(m => m.HomeModule)
      },
      {
        path: 'admin',
        component: LayoutAdminComponent
      },
      {
        path: 'user',
        component: LayoutUserComponent
      },
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      { path: '**', redirectTo: 'home' },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CoreRoutingModule { }
