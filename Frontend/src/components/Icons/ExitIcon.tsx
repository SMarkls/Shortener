import { ReactElement } from 'react';

export const ExitIcon = (props: React.SVGProps<SVGSVGElement>): ReactElement => {
  return (
    <div className="hoverable-icon">
      <svg width="28" height="29" viewBox="0 0 28 29" fill="none" xmlns="http://www.w3.org/2000/svg" {...props}>
        <g clipPath="url(#clip0_9_94)">
          <path
            d="M19.0381 21.3528V25.3909C19.0381 25.9264 18.8307 26.44 18.4617 26.8186C18.0926 27.1972 17.592 27.41 17.07 27.41H3.29329C2.77132 27.41 2.27073 27.1972 1.90164 26.8186C1.53255 26.44 1.3252 25.9264 1.3252 25.3909V3.18115C1.3252 2.64565 1.53255 2.1321 1.90164 1.75345C2.27073 1.3748 2.77132 1.16208 3.29329 1.16208H17.07C17.592 1.16208 18.0926 1.3748 18.4617 1.75345C18.8307 2.1321 19.0381 2.64565 19.0381 3.18115V7.21929"
            stroke="current"
            strokeWidth="2"
            strokeLinecap="round"
            strokeLinejoin="round"
          />
          <path d="M13.1338 14.286H26.9105" stroke="current" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />
          <path d="M22.9742 10.2479L26.9104 14.286L22.9742 18.3242" stroke="current" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />
        </g>
        <defs>
          <clipPath id="clip0_9_94">
            <rect width="27.5534" height="28.267" fill="none" transform="translate(0.341187 0.152557)" />
          </clipPath>
        </defs>
      </svg>
    </div>
  );
};
