import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { FructeComponent } from "./fructe.component";
import { FructeDetailsComponent } from "./fructe-details/fructe-details.component";
import { FructeDetailGuard } from "./fructe-details/fructe-details.guard";

@NgModule({
    declarations: [
        FructeComponent,
        FructeDetailsComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        RouterModule.forChild([
            { path: 'fructe', component: FructeComponent },
            {
                path: 'fructe/:id',
                canActivate: [FructeDetailGuard],
                component: FructeDetailsComponent
            }
        ])
    ]
})
export class FructeModule { }