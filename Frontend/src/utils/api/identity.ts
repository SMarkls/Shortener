import instance from './instance';

export const login = async (nickname: string, password: string) => {
  return await instance.post('/user/login', { nickname, password });
};

export const register = async (nickname: string, password: string, accessPassword: string) => {
  return await instance.post('/user/register', { nickname, password, accessPassword });
};

export const refreshToken = async (accessToken: string, refreshToken: string) => {
  return await instance.post('/user/refreshToken', { accessToken, refreshToken });
};
