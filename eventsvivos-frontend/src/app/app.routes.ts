import { Routes } from '@angular/router';
import { ListaEventos } from './components/lista-eventos/lista-eventos';
import { CrearEvento } from './components/crear-evento/crear-evento';
import { CrearReserva } from './components/crear-reserva/crear-reserva';
import { ReporteOcupacion } from './components/reporte-ocupacion/reporte-ocupacion';

export const routes: Routes = [
  { path: '', redirectTo: 'eventos', pathMatch: 'full' },
  { path: 'eventos', component: ListaEventos },
  { path: 'eventos/crear', component: CrearEvento },
  { path: 'eventos/:id/reservar', component: CrearReserva },
  { path: 'eventos/:id/reporte', component: ReporteOcupacion }
];