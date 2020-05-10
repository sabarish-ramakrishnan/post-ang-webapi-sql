import { AuthService } from './../auth/auth.service';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  isAuthenticated = false;
  isOfficeflag = environment.isOffice;
  constructor(public authService: AuthService) {}

  ngOnInit() {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.authService.userAuthenticatedListener().subscribe(userData => {
      this.isAuthenticated = userData != null;
    });
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  onSignOut() {
    this.authService.signOut();
  }
}
