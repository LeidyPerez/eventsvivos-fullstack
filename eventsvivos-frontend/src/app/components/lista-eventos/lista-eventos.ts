import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { EventoService } from '../../services/evento';
import { DatePipe } from '@angular/common'; 

@Component({
  selector: 'app-lista-eventos',
  imports: [DatePipe, RouterLink, FormsModule],
  templateUrl: './lista-eventos.html',
  styleUrl: './lista-eventos.scss'
})
export class ListaEventos implements OnInit {
  eventos: any[] = [];
  filtros = { tipo: '', estado: '', titulo: '' };
  mensaje = '';

  constructor(private eventoService: EventoService) {}

  ngOnInit() {
    this.cargarEventos();
  }

  cargarEventos() {
    this.eventoService.listarEventos(this.filtros).subscribe({
      next: (data) => this.eventos = data,
      error: (err) => this.mensaje = 'Error al cargar eventos'
    });
  }

  filtrar() {
    this.cargarEventos();
  }

  limpiarFiltros() {
    this.filtros = { tipo: '', estado: '', titulo: '' };
    this.cargarEventos();
  }
}