import { NgModule } from "@angular/core";
import { LegumeComponent } from "./legume.component";
import { LegumeDetailsComponent } from "./legume-details/legume-details.component";
import { RouterModule } from "@angular/router";
import { ProductDetailGuard } from "./legume-details/legume-details.guard";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";

@NgModule({
    declarations: [
        LegumeComponent,
        LegumeDetailsComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        RouterModule.forChild([
            { path: 'legume', component: LegumeComponent },
            {
                path: 'legume/:id',
                canActivate: [ProductDetailGuard],
                component: LegumeDetailsComponent
            }
        ])
    ]
})
export class ProductModule { }