import { Component, signal } from '@angular/core';
import { MaterialModule } from '../../material.module';
import { RegisterComponent } from '../account/register/register';

@Component({
  selector: 'app-home-component',
  imports: [MaterialModule, RegisterComponent],
  standalone: true,
  templateUrl: './home-component.html',
  styleUrl: './home-component.scss',
})
export class HomeComponent {
  protected registerMode = signal(false);

  showRegister() {
    this.registerMode.set(true);
  }
}


