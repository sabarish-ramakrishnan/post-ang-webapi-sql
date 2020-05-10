import { AuthService } from 'src/app/auth/auth.service';
import { Post } from './post.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Subject } from 'rxjs';
import { Router } from '@angular/router';
import { ApiResponseModel } from '../shared/api-response.model';

const BACKEND_URL = environment.apiUrl;
@Injectable()
export class PostService {
  storedPosts: Post[] = [];
  private postChanged = new Subject<Post[]>();
  constructor(
    private http: HttpClient,
    private router: Router,
    private authservice: AuthService
  ) {}

  getPosts() {
    return this.http.get<Post[]>(BACKEND_URL + 'posts').subscribe(resData => {
      this.storedPosts = resData;
      this.postChanged.next([...this.storedPosts]);
    });
  }

  getPostById(id: number) {
    return this.http.get<Post>(BACKEND_URL + 'posts/' + id);
  }

  getPostChangedListener() {
    return this.postChanged.asObservable();
  }

  addPost(newPost: Post) {
    newPost.userId = this.authservice.getUserData().userId;
    return this.http
      .post<Post>(BACKEND_URL + 'posts', newPost)
      .subscribe(createdPost => {
        console.log(createdPost);
        this.storedPosts.push(createdPost);
        this.postChanged.next([...this.storedPosts]);
        this.router.navigate(['/', 'posts']);
      });
  }

  updatePost(id: number, post: Post) {
    post.userId = this.authservice.getUserData().userId;
    post.id = id;
    return this.http
      .put<ApiResponseModel>(BACKEND_URL + 'posts/' + id, post)
      .subscribe(resData => {
        const index = this.storedPosts.indexOf(post);
        this.storedPosts[index] = post;
        this.postChanged.next([...this.storedPosts]);

        this.router.navigate(['/', 'posts']);
      });
  }

  deletePost(id: number) {
    this.http
      .delete<ApiResponseModel>(BACKEND_URL + 'posts/' + id)
      .subscribe(resData => {
        this.storedPosts = this.storedPosts.filter(x => x.id !== id);
        this.postChanged.next([...this.storedPosts]);
      });
  }
}
