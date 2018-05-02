import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';

import { FetchPetsComponent } from './components/fetchpets/fetchpets.component';
import { GroupByPipe } from './components/Utils/utils';
@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        FetchPetsComponent,
        GroupByPipe,
        

    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: FetchPetsComponent },

            
            { path: '**', redirectTo: 'home' }
        ])
    ]
})
export class AppModuleShared {
}
