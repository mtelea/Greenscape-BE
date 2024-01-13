import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { LoginComponent } from './login/login.component';
import { SignupComponent } from './signup/signup.component';
import { ProfileComponent } from './profile/profile.component';
import { LegumeComponent } from './legume/legume.component';
import { FructeComponent } from './fructe/fructe.component';
import { FloriComponent } from './flori/flori.component';
import { LegumeDetailsComponent } from './legume/legume-details/legume-details.component';
import { ProductModule } from './legume/product.module';
import { FructeDetailsComponent } from './fructe/fructe-details/fructe-details.component';
import { FructeModule } from './fructe/fructe.module';
import { FloriDetailsComponent } from './flori/flori-details/flori-details.component';
import { FloriModule } from './flori/flori.module';
import { AdminComponent } from './admin/admin.component';
import { AdminDetailsComponent } from './admin/admin-details/admin-details.component';
import { AdminModule } from './admin/admin.module';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    LoginComponent,
    SignupComponent,
    ProfileComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'login', component: LoginComponent },
      { path: 'profil', component: ProfileComponent },
      { path: 'signup', component: SignupComponent },
      { path: 'legume', component: LegumeComponent },
      { path: 'fructe', component: FructeComponent },
      { path: 'flori', component: FloriComponent },
      { path: 'admin', component: AdminComponent },
    ]),
    ProductModule,FructeModule,FloriModule,AdminModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
