import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

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
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: _ => {
        this.router.navigateByUrl('/members');
        this.model = {};
      }
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
