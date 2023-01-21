import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { ChatAppComponent } from './chat-app/chat-app.component';

const routes: Routes = [
  { path: '', redirectTo: 'chat-app', pathMatch: 'full' },
  { path: 'chat-app', component: ChatAppComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
