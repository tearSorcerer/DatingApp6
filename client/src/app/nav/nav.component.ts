import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { ListsComponent } from '../lists/lists.component';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  constructor(public accountService: AccountService, private router: Router, 
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.accountService.currentUser$.subscribe(
      as => {
        this.model = as;
      }
    )
    
  }

  login() {
    this.accountService.login(this.model).subscribe(response => {
      this.router.navigateByUrl('/members');
    })
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/')
  }
}

//   username: string = '';

//   constructor(public accountService: AccountService, 
//               private router: Router, private toastr: ToastrService) { }

//   ngOnInit(): void {
//     if (localStorage.getItem('user') == null) return;
//     const userObj = JSON.parse(localStorage?.getItem('user') || '{}');
    
//     this.username = userObj.username;
//     this.user = userObj;
  
//   }

//   login() {
//     this.accountService.login(this.user).subscribe(
//       {
//         next: _ => this.router.navigateByUrl('/members'),
//       }
//     );

//     const userObj = JSON.parse(localStorage?.getItem('user') || '{}');
    
//     this.username = userObj.username;
//     this.user = userObj;
//   }

//   logout() {
//     this.accountService.logout();
//     this.router.navigateByUrl('/');
//   }

// }
