import { Routes } from '@angular/router';
import { ListaEventos } from './components/lista-eventos/lista-eventos';
import { CrearEvento } from './components/crear-evento/crear-evento';
import { CrearReserva } from './components/crear-reserva/crear-reserva';
import { ReporteOcupacion } from './components/reporte-ocupacion/reporte-ocupacion';
import { Login } from './components/login/login';
import { authGuard } from './guards/auth-guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'eventos', component: ListaEventos, canActivate: [authGuard] },
  { path: 'eventos/crear', component: CrearEvento, canActivate: [authGuard] },
  { path: 'eventos/:id/reservar', component: CrearReserva, canActivate: [authGuard] },
  { path: 'eventos/:id/reporte', component: ReporteOcupacion, canActivate: [authGuard] }
];