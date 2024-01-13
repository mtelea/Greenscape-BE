import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { FloriComponent } from "./flori.component";
import { FloriDetailsComponent } from "./flori-details/flori-details.component";
import { FloriDetailGuard } from "./flori-details/flori-details.guard";

@NgModule({
    declarations: [
        FloriComponent,
        FloriDetailsComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        RouterModule.forChild([
            { path: 'flori', component: FloriComponent },
            {
                path: 'flori/:id',
                canActivate: [FloriDetailGuard],
                component: FloriDetailsComponent
            }
        ])
    ]
})
export class FloriModule { }