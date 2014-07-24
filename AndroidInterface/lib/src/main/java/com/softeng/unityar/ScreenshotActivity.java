package com.softeng.unityar;

import android.content.Intent;
import android.media.MediaScannerConnection;
import android.net.Uri;
import android.util.Log;

import com.qualcomm.QCARUnityPlayer.QCARPlayerNativeActivity;

public class ScreenshotActivity extends QCARPlayerNativeActivity {
    String emailAddress;

    public void sendEmail(String emailAddress, String filename) {
        Log.i("UnityAR", "Starting intent");
        Log.i("UnityAR", filename);

        this.emailAddress = emailAddress;

        MediaScannerConnection.scanFile(this.getApplicationContext(),
                new String[]{filename}, new String[]{"image/png"},

            new MediaScannerConnection.OnScanCompletedListener() {
                @Override
                public void onScanCompleted(String s, Uri uri) {
                    Intent emailIntent = new Intent(android.content.Intent.ACTION_SEND);
                    emailIntent.setType("image/png");
                    emailIntent.putExtra(android.content.Intent.EXTRA_EMAIL, new String[]{ScreenshotActivity.this.emailAddress});
                    emailIntent.putExtra(android.content.Intent.EXTRA_SUBJECT, "Unity AR Testing Result");
                    emailIntent.putExtra(android.content.Intent.EXTRA_TEXT, "");
                    emailIntent.putExtra(Intent.EXTRA_STREAM, uri);
                    startActivity(Intent.createChooser(emailIntent, "Send mail..."));
                }
            });
    }
}
