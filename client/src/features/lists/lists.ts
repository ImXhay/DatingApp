import { Component, inject, OnInit, signal } from '@angular/core';
import { LikeService } from '../../core/services/like-service';
import { Member } from '../../types/member';
import { MemberCard } from '../members/member-card/member-card';
import { PaginatedResult, Pagination } from '../../types/pagination';
import { Paginator } from '../../shared/paginator/paginator';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-lists',
  imports: [MemberCard, Paginator, FormsModule],
  templateUrl: './lists.html',
  styleUrl: './lists.css',
})
export class Lists implements OnInit {
  private likeService = inject(LikeService);
  protected paginatedResult = signal<PaginatedResult<Member> | null>(null);
  protected predicate = 'liked';

  // Pagination properties
  protected pageNumber = 1;
  protected pageSize = 5;

  tabs = [
    { label: 'Liked', value: 'liked' },
    { label: 'Liked me', value: 'likedBy' },
    { label: 'Mutual', value: 'mutual' },
  ];

  ngOnInit(): void {
    this.loadLikes();
  }

  setPredicate(predicate: string) {
    if (this.predicate !== predicate) {
      this.predicate = predicate;
      this.pageNumber = 1;
      this.loadLikes();
    }
  }

  loadLikes() {
    this.likeService.getLikes(this.predicate, this.pageNumber, this.pageSize).subscribe({
      next: (response) => this.paginatedResult.set(response),
    });
  }
  onPageChanged(event: { pageNumber: number; pageSize: number }) {
    this.pageSize = event.pageSize;
    this.pageNumber = event.pageNumber;
    this.loadLikes();
  }
}
