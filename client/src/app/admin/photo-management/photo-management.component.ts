import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Photo } from 'src/app/_models/photo';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-photo-management',
  templateUrl: './photo-management.component.html',
  styleUrls: ['./photo-management.component.css']
})
export class PhotoManagementComponent {
  photos: Photo[] = [];

  constructor(private adminService: AdminService, private toastr: ToastrService) { 
    this.loadPhotosToBeApproved();
  }

  approvePhoto(photoId: number) {
    this.adminService.approvePhoto(photoId).subscribe({
      next: () => {
        this.photos?.splice(this.photos.findIndex(photo => photo.id === photoId), 1)
      }
    })

    this.toastr.success('Photo approved');
  }

  rejectPhoto(photoId: number) {
    this.adminService.rejectPhoto(photoId).subscribe({
      next: () => {
        this.photos?.splice(this.photos.findIndex(photo => photo.id === photoId), 1)
      }
    })
    this.toastr.success('Photo rejected');
  }

  loadPhotosToBeApproved() {
    this.adminService.getPhotosForApproval().subscribe({
      next: photos =>  {
        this.photos = photos;
  }})

  }

}
