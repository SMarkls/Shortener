import { create } from 'zustand';
import { persist } from 'zustand/middleware';

type IdentityStore = {
  accessToken: string;
  refreshToken: string;
  userName: string;
  expiresAt: number;
  setTokens: (tokens: Pick<IdentityStore, 'accessToken' | 'refreshToken'>) => void;
  setUserName: (userName?: string) => void;
  setExpiration: (expirseAt: number) => void;
};

export const useIdentity = create(
  persist<IdentityStore>(
    (set) => ({
      accessToken: '',
      refreshToken: '',
      userName: '',
      expiresAt: 0,
      setTokens: (tokens: Pick<IdentityStore, 'accessToken' | 'refreshToken'>) => set((state) => ({ ...state, ...tokens })),
      setUserName: (userName?: string) => set((state) => ({ ...state, userName })),
      setExpiration: (expirseAt: number) => set((state) => ({ ...state, expiresAt: expirseAt })),
    }),
    { name: 'identityStore' }
  )
);
