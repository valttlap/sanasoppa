// eslint-disable-next-line @typescript-eslint/no-var-requires
const dotenv = require('dotenv');
// eslint-disable-next-line @typescript-eslint/no-var-requires
const { writeFile } = require('fs');
// eslint-disable-next-line @typescript-eslint/no-var-requires
const { promisify } = require('util');
// eslint-disable-next-line @typescript-eslint/no-var-requires
const { mkdir } = require('fs');

dotenv.config();

const writeFilePromisified = promisify(writeFile);

const targetDir = './src/environments/';

// Make directory if it doesn't exist
mkdir(targetDir, { recursive: true }, (err: any) => {
    if (err) throw err;
});

const targetPath = `${targetDir}environment.ts`;

const envConfigFile = `export const environment = {
    production: false,
    auth0: {
      domain: '${process.env['AUTH0_DOMAIN']}',
      clientId: '${process.env['AUTH0_CLIENT_ID']}',
      authorizationParams: {
        audience: '${process.env['AUTH0_AUDIENCE']}',
        redirect_uri: '${process.env['AUTH0_CALLBACK_URL']}',
      },
      errorPath: '/callback',
    },
    api: {
      apiUrl: '${process.env['API_SERVER_URL']}',
      hubUrl: '${process.env['HUB_SERVER_URL']}',
    },
    recaptcha: {
      siteKey: '${process.env['RECAPTCHA_SITE_KEY']}',
    },
  };
  `;

(async () => {
  try {
    await writeFilePromisified(targetPath, envConfigFile);
  } catch (err) {
    console.error(err);
    throw err;
  }
})();
