import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'FBS_Front';
  constructor(private router: Router) {}

  redirectToSearch() {
    this.router.navigate(['/search']);
  }

  
}
