import { ReactElement } from 'react';

export const EnterIcon = (props: React.SVGProps<SVGSVGElement>): ReactElement => {
  return (
    <div className="hoverable-icon">
      <svg width="29" height="29" viewBox="0 0 29 29" fill="none" xmlns="http://www.w3.org/2000/svg" {...props}>
        <g clipPath="url(#clip0_9_89)">
          <path
            d="M19.3779 21.3528V25.3909C19.3779 25.9264 19.1706 26.44 18.8015 26.8186C18.4324 27.1972 17.9318 27.41 17.4098 27.41H3.63314C3.11116 27.41 2.61057 27.1972 2.24148 26.8186C1.87239 26.44 1.66504 25.9264 1.66504 25.3909V3.18115C1.66504 2.64565 1.87239 2.1321 2.24148 1.75345C2.61057 1.3748 3.11116 1.16208 3.63314 1.16208H17.4098C17.9318 1.16208 18.4324 1.3748 18.8015 1.75345C19.1706 2.1321 19.3779 2.64565 19.3779 3.18115V7.21929"
            stroke="current"
            strokeWidth="2"
            strokeLinecap="round"
            strokeLinejoin="round"
          />
          <path d="M27.2503 14.286H11.5055" stroke="current" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />
          <path d="M15.4417 10.2479L11.5055 14.286L15.4417 18.3242" stroke="current" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />
        </g>
        <defs>
          <clipPath id="clip0_9_89">
            <rect width="27.5534" height="28.267" fill="none" transform="translate(0.68103 0.152557)" />
          </clipPath>
        </defs>
      </svg>
    </div>
  );
};
