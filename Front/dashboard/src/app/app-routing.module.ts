import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { DashboardHomeComponent } from './pages/dashboard-home/dashboard-home.component';
import { ProductsComponent } from './pages/products/products.component';
import { CategoriesComponent } from './pages/categories/categories.component';
import { CouponsComponent } from './pages/coupons/coupons.component';
import { ClientsComponent } from './pages/clients/clients.component';
import { SettingsComponent } from './pages/settings/settings.component';
import { SitesComponent } from './pages/sites/sites.component';
import { SiteDetalheComponent } from './pages/sites/site-detalhe/site-detalhe.component';
import { SiteListaComponent } from './pages/sites/site-lista/site-lista.component';
import { PedidosComponent } from './pages/pedidos/pedidos.component';
import { PedidoDetalheComponent } from './pages/pedidos/pedido-detalhe/pedido-detalhe.component';
import { PedidoListaComponent } from './pages/pedidos/pedido-lista/pedido-lista.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' }, // Redireciona para login
  { path: 'login', component: LoginComponent },
  { path: 'dashboard', component: DashboardHomeComponent },
  { path: 'sites', redirectTo: 'sites/lista' },
  {
    path: 'sites',
    component: SitesComponent,
    children: [
      { path: 'detalhe/:id', component: SiteDetalheComponent },
      { path: 'detalhe', component: SiteDetalheComponent },
      { path: 'lista', component: SiteListaComponent },
    ],
  },
  { path: 'products', component: ProductsComponent },
  { path: 'categories', component: CategoriesComponent },
  { path: 'coupons', component: CouponsComponent },
  { path: 'pedidos', redirectTo: 'pedidos/lista' },
  { path: 'pedidos', 
    component: PedidosComponent, 
    children: [
        { path: 'detalhe/:id', component: PedidoDetalheComponent },
        { path: 'detalhe', component: PedidoDetalheComponent },
        { path: 'lista', component: PedidoListaComponent },
      ],
  },
  { path: 'clients', component: ClientsComponent },
  { path: 'settings', component: SettingsComponent },
  { path: '**', redirectTo: '/login' } // Página não encontrada vai para login
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }