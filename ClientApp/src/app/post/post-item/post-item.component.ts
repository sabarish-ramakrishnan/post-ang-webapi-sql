import { PostService } from './../post.service';
import { Post } from './../post.model';
import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-post-item',
  templateUrl: './post-item.component.html',
  styleUrls: ['./post-item.component.css']
})
export class PostItemComponent implements OnInit {
  @Input() postItem: Post;
  @Input() isAuthenticated = false;
  @Input() loggedInUserId = 0;
  isOfficeflag = environment.isOffice;
  constructor(private router: Router, private psService: PostService) {}

  ngOnInit() {}

  onEditItem(id: number) {
    this.router.navigate(['/', 'edit-post', id]);
  }

  onDeleteItem(id: number) {
    this.psService.deletePost(id);
  }
}
