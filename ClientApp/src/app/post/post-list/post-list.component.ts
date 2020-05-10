import { AuthService } from './../../auth/auth.service';
import { PostService } from './../post.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { Post } from '../post.model';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.css']
})
export class PostListComponent implements OnInit, OnDestroy {
  postList: Post[] = [];
  isLoading = false;
  isAuthenticated = false;
  loggedInUserId = 0;
  isOfficeflag = environment.isOffice;
  private psSubscription: Subscription;
  constructor(
    private psService: PostService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.isLoading = true;

    this.psService.getPosts();
    this.psSubscription = this.psService
      .getPostChangedListener()
      .subscribe(newPosts => {
        setTimeout(() => {
          this.postList = newPosts;
          this.isLoading = false;
        }, 0);
      });

    this.isAuthenticated = this.authService.isAuthenticated();
    if (this.isAuthenticated) {
      this.loggedInUserId = this.authService.getUserData().userId;
    }
    this.authService.userAuthenticatedListener().subscribe(userData => {
      if (userData != null) {
        this.isAuthenticated = true;
        this.loggedInUserId = userData.userId;
      } else {
        this.isAuthenticated = false;
        this.loggedInUserId = 0;
      }
    });
  }

  ngOnDestroy() {
    this.psSubscription.unsubscribe();
  }
}
