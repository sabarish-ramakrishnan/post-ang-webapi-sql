import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {
  constructor(private authService: AuthService) {}

  ngOnInit() {}
  onSubmit(f: NgForm) {
    console.log(f.value);
    this.authService.signup(f.value.email, f.value.password);
  }
}
