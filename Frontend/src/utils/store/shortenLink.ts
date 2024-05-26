import { create } from 'zustand';

export type ShortenLink = {
  id: number;
  token: string;
  fullLink: string;
  countOfRedirections: number;
};

type ShortenLinkStore = {
  shortenLinks: ShortenLink[];
};

export const useShortenLink = create<ShortenLinkStore>((set) => ({
  shortenLinks: [],
  addShortenLink: (shortenLink: ShortenLink) => set((state: ShortenLinkStore) => ({ shortenLinks: [...state.shortenLinks, shortenLink] })),
  removeShortenLink: (id: number) => set((state: ShortenLinkStore) => ({ shortenLinks: state.shortenLinks.filter((l) => l.id !== id) })),
}));
