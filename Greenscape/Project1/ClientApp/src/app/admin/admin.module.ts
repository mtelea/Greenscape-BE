import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { AdminComponent } from "./admin.component";
import { AdminDetailsComponent } from "./admin-details/admin-details.component";
import { AdminDetailGuard } from "./admin-details/admin-details.guard";

@NgModule({
    declarations: [
        AdminComponent,
        AdminDetailsComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule.forChild([
            { path: 'admin', component: AdminComponent },
            {
                path: 'admin/:id',
                canActivate: [AdminDetailGuard],
                component: AdminDetailsComponent
            }
        ])
    ]
})
export class AdminModule { }