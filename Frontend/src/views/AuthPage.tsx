import { ReactElement, useState } from 'react';
import { Header } from '../components/Header/Header';
import { Input } from '../components/Input/Input';
import { Button } from '../components/Button/Button';
import { register, login as loginRequest } from '../utils/api/identity';
import { useIdentity } from '../utils/store/identity';
import { useNavigate } from 'react-router-dom';

export const AuthPage = (): ReactElement => {
  const [isRegister, setRegister] = useState(false);
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');
  const [accessPassword, setAccessPassword] = useState('');
  const [error, setError] = useState('');
  const setTokens = useIdentity((state) => state.setTokens);
  const setUserName = useIdentity((state) => state.setUserName);
  const setExpiration = useIdentity((state) => state.setExpiration);
  const setNavigtion = useNavigate();

  const authBtnClicked = async () => {
    if (isRegister) {
      const data = await register(login, password, accessPassword);
      if (data.status !== 200) {
        console.error('Ошибка регистрации');
        setError(data.data);
        return;
      }

      setTokens(data.data);
      const jwt = data.data.accessToken;
      setNameAndExpiration(jwt);
      setNavigtion('/');
      return;
    }

    const data = await loginRequest(login, password);
    if (data.status !== 200) {
      console.error('Ошибка авторизации');
      setError('Неверный логин или пароль');
      return;
    }

    setTokens(data.data);
    const jwt = data.data.accessToken;
    setNameAndExpiration(jwt);
    setNavigtion('/');
  };

  const setNameAndExpiration = (token: string) => {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map(function (c) {
          return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        })
        .join('')
    );
    const userData = JSON.parse(jsonPayload);
    setUserName(userData.sub);
    const expiration = userData.exp * 1000;
    setExpiration(expiration);
  };

  return (
    <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', width: '80%', gap: '2vw' }}>
      <div>{error}</div>
      <Header>{isRegister ? 'Регистрация' : 'Авторизация'}</Header>
      <Input type="text" placeholder="Логин" onChange={(value) => setLogin(value)} />
      <Input type="password" placeholder="Пароль" onChange={(value) => setPassword(value)} />
      {isRegister && <Input type="password" placeholder="Повторите пароль" onChange={(value) => setAccessPassword(value)} />}
      <div style={{ display: 'flex', flexDirection: 'column', gap: '1vw' }}>
        <Button onClick={authBtnClicked}>{isRegister ? 'Регистрация' : 'Войти'}</Button>
        <Button onClick={() => setRegister(!isRegister)}>{isRegister ? 'Уже есть аккаунт?' : 'Нет аккаунта?'}</Button>
      </div>
    </div>
  );
};
