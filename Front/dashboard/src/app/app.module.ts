import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SidebarComponent } from './components/layout/sidebar/sidebar.component';
import { HeaderComponent } from './components/layout/header/header.component';
import { MainContentComponent } from './components/layout/main-content/main-content.component';
import { StatsCardsComponent } from './components/dashboard/stats-cards/stats-cards.component';
import { RecentActivityComponent } from './components/dashboard/recent-activity/recent-activity.component';
import { QuickActionsComponent } from './components/dashboard/quick-actions/quick-actions.component';
import { DashboardHomeComponent } from './pages/dashboard-home/dashboard-home.component';
import { ProductsComponent } from './pages/products/products.component';
import { CategoriesComponent } from './pages/categories/categories.component';
import { CouponsComponent } from './pages/coupons/coupons.component';
import { SettingsComponent } from './pages/settings/settings.component';
import { ClientsComponent } from './pages/clients/clients.component';
import { LoginComponent } from './pages/login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SitesComponent } from './pages/sites/sites.component';
import { HttpClientModule } from '@angular/common/http';
import { SiteListaComponent } from './pages/sites/site-lista/site-lista.component';
import { SiteDetalheComponent } from './pages/sites/site-detalhe/site-detalhe.component';
import { LoadingSpinnerComponent } from './components/layout/loading-spinner/loading-spinner.component';
import { PedidosComponent } from './pages/pedidos/pedidos.component';
import { PedidoListaComponent } from './pages/pedidos/pedido-lista/pedido-lista.component';
import { PedidoDetalheComponent } from './pages/pedidos/pedido-detalhe/pedido-detalhe.component';

@NgModule({
  declarations: [
    AppComponent,
    SidebarComponent,
    HeaderComponent,
    MainContentComponent,
    StatsCardsComponent,
    RecentActivityComponent,
    QuickActionsComponent,
    DashboardHomeComponent,
    ProductsComponent,
    CategoriesComponent,
    CouponsComponent,
    SettingsComponent,
    ClientsComponent,
    LoginComponent,
    SitesComponent,
    SiteListaComponent,
    SiteDetalheComponent,
    LoadingSpinnerComponent,
    PedidosComponent,
    PedidoListaComponent,
    PedidoDetalheComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
