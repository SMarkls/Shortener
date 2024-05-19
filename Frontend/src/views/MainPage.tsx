import { ReactElement, useEffect, useState } from 'react';
import { Input } from '../components/Input/Input';
import { Button } from '../components/Button/Button';
import { Header } from '../components/Header/Header';
import { ShortenLinkItem } from '../components/ShortenLinkItem/ShortenLinkItem';
import { createShortenLink, getShortenLinks } from '../utils/api/shortenLink';
import { ShortenLink } from '../utils/store/shortenLink';
import { useIdentity } from '../utils/store/identity';
import { DropDown } from '../components/DropDown/DropDown';

export const MainPage = (): ReactElement => {
  const [shortenLinks, setShortenLinks] = useState<ShortenLink[]>([]);
  const [fullLink, setFullLink] = useState<string>('');
  const [showFullLink, setShowFullLink] = useState<string>('');
  const [shortLink, setShortLink] = useState<string>('');
  const isAuthorized = useIdentity((state) => !!state.accessToken);

  useEffect(() => {
    if (!isAuthorized) {
      return;
    }

    const fetchData = async () => {
      const response = await getShortenLinks();
      if (response.status != 200) {
        console.error('Ошибка получения ссылок');
        return;
      }

      setShortenLinks(response.data);
    };

    fetchData();
  }, [isAuthorized, shortLink]);

  const sendLink = async () => {
    const response = await createShortenLink(fullLink);
    if (response.status != 200) {
      console.error('Ошибка создания ссылки');
      return;
    }

    console.log(response);
    setShortLink(response.data);
    setShowFullLink(fullLink);
  };

  const onDeleted = (id: number) => {
    setShortenLinks(shortenLinks.filter((x) => x.id !== id));
  };

  return (
    <>
      <Input type="text" onChange={(value) => setFullLink(value)} placeholder="Введите ссылку" />
      <Button onClick={sendLink}>Сократить!</Button>
      {shortLink && <DropDown fullUrl={showFullLink} token={shortLink} />}
      {shortenLinks.length > 0 && (
        <div style={{ marginTop: '10vh' }}>
          <Header>ВАШИ ССЫЛКИ</Header>
        </div>
      )}
      {shortenLinks.map((x, i) => (
        <ShortenLinkItem key={i} countRedirections={x.countOfRedirections} fulllUrl={x.fullLink} token={x.token} id={x.id} cb={onDeleted} />
      ))}
      {/* <ShortenLinkItem countRedirections={42} fulllUrl="vk.com/test" token="testte" id={0} /> */}
    </>
  );
};
