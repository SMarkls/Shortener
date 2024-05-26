import { ReactElement } from 'react';
import style from './Graphic.module.css';
import { Doughnut, Line } from 'react-chartjs-2';
import 'chart.js/auto';

type GraphicProps = {
  statistics: { browser: string; time: Date; currentCount?: number }[];
  countOfUniqueUsers: number;
};

export const Graphic = ({ statistics, countOfUniqueUsers }: GraphicProps): ReactElement => {
  let sum = 0;
  const sortedStatistic = statistics.sort((a, b) => a.time.getTime() - b.time.getTime());
  console.log(sortedStatistic);
  for (const i of sortedStatistic) {
    sum += 1;
    i.currentCount = sum;
  }

  const getBrowsers = () => {
    const browsers: string[] = [];
    for (const i of sortedStatistic) {
      if (!browsers.includes(i.browser)) {
        browsers.push(i.browser);
      }
    }

    const counts: number[] = [];
    for (const i of browsers) {
      const length = sortedStatistic.filter((x) => x.browser === i).length;
      counts.push(length);
    }
    return { browsers, counts };
  };
  const { browsers, counts } = getBrowsers();
  const getRandomColor = () => {
    return `#${Math.floor(Math.random() * 16777215).toString(16)}`;
  };

  const colors = Array.from({ length: counts.length }, () => getRandomColor());

  return (
    <div className={style.graphic}>
      <Line
        data={{ labels: sortedStatistic.map((x) => x.time.toLocaleString()), datasets: [{ label: 'Количество переходов', data: sortedStatistic.map((x) => x.currentCount) }] }}
        options={{ locale: 'ru' }}
      />
      <Doughnut data={{ labels: browsers, datasets: [{ data: counts, backgroundColor: colors }] }} />
      <p>Всего переходов: {sortedStatistic.length}</p>
      <p>Уникальных переходов: {countOfUniqueUsers}</p>
    </div>
  );
};
