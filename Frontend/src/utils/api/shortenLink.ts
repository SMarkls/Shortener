import instance from './instance';
import { useIdentity } from '../store/identity';
import { refreshToken as refreshTokenRequest } from './identity';

export const getShortenLink = async (id: number) => {
  configureHeaders();
  return await instance.get(`/shortenLink/get?id=${id}`);
};

export const getShortenLinks = async () => {
  configureHeaders();
  return await instance.get(`/shortenLink/getList`);
};

export const createShortenLink = async (fullLink: string) => {
  configureHeaders();
  return await instance.post(`/shortenLink/create`, { fullLink });
};

export const deleteShortenLink = async (id: number) => {
  configureHeaders();
  return await instance.delete(`/shortenLink/delete?id=${id}`);
};

export const getFullLink = async (token: string) => {
  return await instance.get(`/shortenLink/getFullLink?token=${token}`);
};

const configureHeaders = () => {
  let accessToken = useIdentity.getState().accessToken;
  let refreshToken = useIdentity.getState().refreshToken;
  let expiresAt = useIdentity.getState().expiresAt;
  if (!expiresAt) {
    return;
  }

  const currentUTC = new Date().getTime();
  if (currentUTC > expiresAt) {
    refreshTokenRequest(accessToken, refreshToken).then((response) => {
      expiresAt = response.data.expiresAt;
      accessToken = response.data.accessToken;
      refreshToken = response.data.refreshToken;
      useIdentity.setState({ accessToken, refreshToken, expiresAt });
    });
  }
  instance.defaults.headers.common['Authorization'] = `Bearer ${accessToken}`;
};
