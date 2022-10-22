export const theme = {
  colors: {
      primaryAccent: '#00A384', // primary green color
      primaryAccentLght: '#00a657', // primary light green color
      secondaryAccent: '#00B2DD', // secondary orange color
      tertiaryAccent: '#00B2DD', // tertiary sky blue color
      primaryBackground: '#fbfbfb', // app background
      secondaryBackground: '#f2f2f2',
      highlight:'##DF7909', // highlight an item (blue)
      error: 'darkred',
      border: '#cccccc', // normal border state
      notice: '#b0b0b0', // gray notice states
      inputFocus: '#006A4D' // input focus color
  },
  font: {
    primary: "'Futura Md BT', helvetica, arial, sans-serif",
    bold: "'Futura Md BT Bold', helvetica, arial, sans-serif",
    size: {
      s: '.8em',
      m: '1em',
      l: '1.2em',
      xl: '1.5em',
    },
    formSize: {
      label: '16px',
      input: '14px'
    },
    weight: {
      normal: 300,
      bolder: 900
    },
    letterSpacing: '0.36',
  },
  media: {
    screen: {
      s: '768px',
    },
  },
  container: {
    maxWidth: '1300px',
    width: '90%'
  }
};

export type Theme = typeof theme;