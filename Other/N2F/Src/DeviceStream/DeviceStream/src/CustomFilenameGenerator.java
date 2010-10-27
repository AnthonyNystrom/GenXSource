package DeviceStream;

import org.red5.server.api.IScope;
import org.red5.server.api.ScopeUtils;
import org.red5.server.api.stream.IStreamFilenameGenerator;

public class CustomFilenameGenerator implements IStreamFilenameGenerator {

    /** Path that will store recorded videos. */
    public static String recordPath;
    /** Path that contains VOD streams. */
    public static String playbackPath;
    
    private String getStreamDirectory(IScope scope) {
		final StringBuilder result = new StringBuilder();
		final IScope app = ScopeUtils.findApplication(scope);
		while (scope != null && scope != app) {
			result.insert(0, "/" + scope.getName());
			scope = scope.getParent();
		}
		return playbackPath + result.toString();
    }

    public String generateFilename(IScope scope, String name, GenerationType type) {
            return generateFilename(scope, name, null, type);
    }

    public String generateFilename(IScope scope, String name, String extension, GenerationType type) {
        String filename;    
        filename = getStreamDirectory(scope) + name;

        if (extension != null)
            // Add extension
            filename += extension;

        return filename;
    }

    public boolean resolvesToAbsolutePath() {
    	return true;
    }
    
    public void setPlaybackPath(String path) {
    	playbackPath = path;
    }

    public void setRecordPath(String path) {
        recordPath = path;
    }

}