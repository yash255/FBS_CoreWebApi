import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/Services/auth.service';
import { UserStoreService } from 'src/app/Services/user-store.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit{

  public users:any = [];
  public role!:string;

  loggedIn:boolean=this.authService.isLoggedIn();
  AdminLoggedIn:boolean=this.authService.isAdminLoggedIn();
  constructor(private authService:AuthService, private userStore: UserStoreService){
    this.loggedIn=this.authService.isLoggedIn();
    this.AdminLoggedIn=this.authService.isAdminLoggedIn();
  }
    ngOnInit() {
      // this.api.getUsers()
      // .subscribe(res=>{
      // this.users = res;
      // });
  
      // this.userStore.getFullNameFromStore()
      // .subscribe(val=>{
      //   const fullNameFromToken = this.auth.getfullNameFromToken();
      //   this.fullName = val || fullNameFromToken
      // });
  
      this.userStore.getRoleFromStore()
      .subscribe(val=>{
        const roleFromToken = this.authService.getRoleFromToken();
        this.role = val || roleFromToken;
      })
    }
  logout(){
    this.loggedIn=false;
    this.authService.signOut();
  }
  
}
