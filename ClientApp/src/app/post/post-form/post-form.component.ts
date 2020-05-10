import { Post } from './../post.model';
import { PostService } from './../post.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-post-form',
  templateUrl: './post-form.component.html',
  styleUrls: ['./post-form.component.css']
})
export class PostFormComponent implements OnInit {
  postForm: FormGroup;
  id: number;
  postItem: Post;
  editMode = false;
  invalidImage = false;
  imagePreview: string | ArrayBuffer;
  constructor(
    private psService: PostService,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    this.postForm = new FormGroup({
      title: new FormControl(null, Validators.required),
      content: new FormControl(null, Validators.required),
      imagePath: new FormControl(null, Validators.required)
    });
    this.activatedRoute.params.subscribe(params => {
      if (params['id']) {
        this.id = +params['id'];
        this.psService.getPostById(this.id).subscribe(resData => {
          this.postItem = resData;
          this.postForm.patchValue({
            title: this.postItem.title,
            content: this.postItem.content,
            imagePath: this.postItem.imagePath
          });
          this.postForm.updateValueAndValidity();
          this.editMode = true;
        });
      } else {
        this.editMode = false;
      }
    });
  }

  onSubmit() {
    if (this.editMode) {
      this.psService.updatePost(this.id, this.postForm.value);
    } else {
      this.psService.addPost(this.postForm.value);
    }
  }

  onImagePicked(event: Event) {
    const file = (event.target as HTMLInputElement).files[0];
    console.log(file);
    this.postForm.patchValue({ imagePath: file });
    this.postForm.get('imagePath').updateValueAndValidity();
    if (file.type.includes('image')) {
      this.invalidImage = false;
    } else {
      this.invalidImage = true;
    }
    const reader = new FileReader();
    reader.onload = () => {
      this.imagePreview = reader.result;
    };
    reader.readAsDataURL(file);
  }
}
