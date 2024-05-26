import { ReactElement } from 'react';
import style from './Navbar.module.css';
import Logo from '../../assets/Logo.svg';
import { ExitIcon } from '../Icons/ExitIcon';
import { EnterIcon } from '../Icons/EnterIcon';
import { useIdentity } from '../../utils/store/identity';
import { Link, useNavigate } from 'react-router-dom';

export const Navbar = (): ReactElement => {
  const setNavigtion = useNavigate();
  const userName = useIdentity((state) => state.userName);
  const setUserName = useIdentity((state) => state.setUserName);
  const setTokens = useIdentity((state) => state.setTokens);
  const accessToken = useIdentity((state) => state.accessToken);

  const onIdentityButtonClicked = () => {
    if (userName) {
      setUserName(undefined);
      setTokens({ accessToken: '', refreshToken: '' });
      window.location.reload();
      return;
    }

    setNavigtion('/auth');
  };

  return (
    <>
      <div className={style.navbar}>
        <Link to={'/'}>
          <img className={style.logo} src={Logo} draggable={false} />
        </Link>
        <div style={{ display: 'flex', alignItems: 'center', gap: '1vw', cursor: 'pointer' }}>
          <div className={style.userName} onClick={onIdentityButtonClicked}>
            {(accessToken && userName) || 'Войти'}
          </div>
          <div onClick={onIdentityButtonClicked} draggable={false}>
            {userName ? <ExitIcon /> : <EnterIcon />}
          </div>
        </div>
      </div>
    </>
  );
};
